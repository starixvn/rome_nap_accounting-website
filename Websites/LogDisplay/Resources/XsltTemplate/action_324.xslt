<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:param name="IsInternal" select="1"></xsl:param>
  <xsl:output method="html"/>

  <xsl:template match="acc_customerupdmember_minmaxbet151_51">
    <table class="tblDesc">
      <tr>

        <td class="colW1">{lbl:MinBv151_51}</td>
        <td class="colW2">
          <xsl:value-of select="@minb" />
        </td>
        <td class="colW1">{lbl:MaxBv151_51}</td>
        <td class="colW2">
          <xsl:value-of select="@maxb" />
        </td>
        <td class="colW1">{lbl:MPMv151_51}</td>
        <td class="colW2">
          <xsl:value-of select="@mpm" />
        </td>
      </tr>
      <tr>
        <td class="colW1">{lbl:SubAccName}</td>
        <td class="colW2">
          <xsl:value-of select="@subaccname" />
        </td>
        <td class="colW1">{lbl:IP}</td>
        <td class="colW2">
          <xsl:value-of select="@ip" />
        </td>
        <td class="colW1">{lbl:Language}</td>
        <td class="colW2">
          <xsl:value-of select="@language" />
        </td>
      </tr>
      <tr>
      </tr>
      <tr>
        <td class="colW1">{lbl:ErrCode}</td>
        <td class="colW2">
          <xsl:value-of select="@errcode" />
        </td>
        <td class="colW1">{lbl:ErrMsg}</td>
        <td class="colW2">
          <xsl:value-of select="@errmsg" />
        </td>
        <td class="colW1"></td>
        <td class="colW2">
        </td>
      </tr>
    </table>
  </xsl:template>



</xsl:stylesheet>
