<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
    <xsl:param name="IsInternal" select="1"></xsl:param>
    <xsl:output method="html"/>

    <xsl:template match="dba_customerupdmovemaster">
        <table class="tblDesc">
            <tr>
              <td class="colW1">{lbl:MasterName}</td>
              <td class="colW2">
                <xsl:value-of select="@mastername" />
              </td>
              
              <td class="colW1">{lbl:OldSuperId}</td>
              <td class="colW2">
                <xsl:value-of select="@oldsuperid" />
              </td>

                
                <td class="colW1">{lbl:NewSuperName}</td>
                <td class="colW2">
                    <xsl:value-of select="@newsupername" />
                </td>

            </tr>
            <tr>
                <td class="colW1">{lbl:NewSuperId}</td>
                <td class="colW2">
                    <xsl:value-of select="@newsuperid" />
                </td>
                <td class="colW1">{lbl:IP}</td>
                <td class="colW2">
                    <xsl:value-of select="@ip" />
                </td>
                <td class="colW1"></td>
                <td class="colW2"></td>
            </tr>
        </table>

    </xsl:template>
</xsl:stylesheet>
