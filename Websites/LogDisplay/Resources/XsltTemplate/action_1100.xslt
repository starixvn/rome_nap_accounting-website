<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="acc_customerinsmember_com">
        <table class="tblDesc">
            <tr>                 
                <td class="colW1">
                    {lbl:disabledHR}
                </td>
                <td class="colW2">
                    <xsl:value-of select="@disablehorseracing" />
                </td>
				        <td class="colW1">
                  {lbl:disableot}
                </td>
				        <td class="colW2">
					        <xsl:value-of select="@disableot" />
				        </td>
				        <td class="colW1">
                  {lbl:disable151_51}
                </td>
				        <td class="colW2">
					        <xsl:value-of select="@disable151_51" />
				        </td>
            </tr>
            <tr>
              <td class="colW1">
                {lbl:Disable152_AU}
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable152_41" />
              </td>
              <td class="colW1">
                {lbl:Disable152_UK}
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable152_51" />
              </td>
              <td class="colW1">
                {lbl:Disable153_AU}
              </td>
              <td class="colW2">
                <xsl:value-of select="@disable153_51" />
              </td>
            </tr>

          <tr>
            <td class="colW1">
              {lbl:disableCasino}
            </td>
            <td class="colW2">
              <xsl:value-of select="@disablecasino" />
            </td>
            <td class="colW1">       
            </td>
            <td class="colW2">
              <xsl:value-of select="@disablefinancial" />
            </td>
            <td class="colW1">
              {lbl:DisableNumber}
            </td>
            <td class="colW2">
              <xsl:value-of select="@disablenumber" />
            </td>
          </tr>

          <tr>
            <td class="colW1">{lbl:disablebingo}</td>
            <td class="colW2">
              <xsl:value-of select="@disablebingo" />
            </td>
            <td class="colW1">
              {lbl:Internal}
            </td>
            <td class="colW2">
              <xsl:value-of select="@isinternal" />
            </td>            
            <td class="colW1"></td>
            <td class="colW2"></td>
          </tr>
        </table>
    </xsl:template>
</xsl:stylesheet>
